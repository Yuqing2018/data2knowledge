# -*- coding:utf-8 -*-
# noinspection PyBroadException

import os
import sys
import time
import platform
import datetime
import pytz
from urllib.parse import urlparse
import logging
from urllib.parse import urljoin

from selenium import webdriver
from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.support import expected_conditions as ec
from selenium.webdriver.common.by import By
from pyvirtualdisplay import Display

from app.utils.mongo_operator import MongoOperator

# from .config import Config

STROLL_TO_BOTTOM = """
            function scrollToBottom() {
                var Height = document.body.clientHeight,
                    screenHeight = window.innerHeight,
                    INTERVAL = 50,
                    delta = 100,
                    curScrollTop = 0;
                var scroll = function () {
                    //curScrollTop = document.body.scrollTop;
                    curScrollTop = curScrollTop + delta;
                    window.scrollTo(0,curScrollTop);
                };
                var timer = setInterval(function () {
                    var curHeight = curScrollTop + screenHeight;
                    if (curHeight >= Height){
                        clearInterval(timer);
                    }
                    scroll();
                }, INTERVAL)
            }
            scrollToBottom();
            """
STROLL = """
function scrollToBottom() {
    let Height = document.body.clientHeight,
        screenHeight = window.innerHeight,
        INTERVAL = 1000,
        delta = Height,
        curScrollTop = 0;
    let scroll = function () {
        curScrollTop = curScrollTop + delta;
        window.scrollTo(0, curScrollTop);
    };
    let timer = setInterval(function () {
        scroll();
    }, INTERVAL)
}
scrollToBottom();
"""
SYS_INFO = platform.uname()


def remove_list_dup(l):
    new_li = list(set(l))
    new_li.sort(key=l.index)
    return new_li


def parse_url(target_url):
    url_info = {}
    url_obj = urlparse(target_url)
    url_info["scheme"] = url_obj.scheme
    url_info["domain"] = url_obj.netloc
    url_info["path"] = url_obj.path
    url_info["params"] = url_obj.params
    url_info["query"] = url_obj.query
    url_info["fragment"] = url_obj.fragment
    return url_info


# remove if a tag is comment, tag, comment,
def remove_non_article_link(links_list):
    exclude_list = ["/tag/", "/author/", "/video/", "/tags."]
    for link in links_list.copy():
        for name in exclude_list:
            if name in link:
                links_list.remove(link)
    return links_list


class InfoFilter(logging.Filter):
    def filter(self, record):
        """
        only use INFO log
        """
        if logging.INFO <= record.levelno < logging.ERROR:
            return super().filter(record)
        else:
            return 0


logger = logging.getLogger(__name__)
logger.setLevel(level=logging.INFO)
formater = logging.Formatter('%(asctime)s %(levelname)s %(process)d %(thread)d %(pathname)s %(lineno)s --> %(message)s')
stream_handler = logging.StreamHandler(stream=sys.stdout)
stream_handler.setFormatter(formater)
logger.addHandler(stream_handler)


class ArticlesSpider(object):
    def __init__(self, page_url=None, target_xpath=None, stroll=False, article_title_xpath=None, article_content_xpath=None, app_config=None, sched_id=None):
        self.page_url = page_url
        self.stroll = stroll
        self.xpath = target_xpath
        self.article_title_xpath = article_title_xpath
        self.article_content_xpath = article_content_xpath
        self.sched_id = sched_id
        self.mongo_host = app_config['MONGODB_HOST']
        self.mongo_port = app_config['MONGODB_PORT']
        self.info_db = app_config['MONGODB_INFO_DATABASE']
        self.info_col = app_config['ARTICLE_COLL_NAME']
        self.log_col = app_config['LOG_COLL_NAME']
        self.mo = MongoOperator(host=self.mongo_host, port=self.mongo_port, db=self.info_db)
        self.target_urls = ArticlesSpider.get_target_urls(url=self.page_url, stroll=self.stroll, target_xpath=self.xpath)

    @property
    def ensure_now_rfc822(self):
        time_zone = pytz.timezone('Asia/Shanghai')
        # time_zone = tz.tzlocal()
        dt = datetime.datetime.now(tz=time_zone)
        dt = dt.strftime('%a, %d %b %Y %H:%M:%S %z')
        return dt

    @staticmethod
    def get_target_urls(url=None, stroll=False, target_xpath=None):
        """
        :param url: page url
        :param stroll: if the target conten of the page is endless
        :param target_xpath:uUniquely identifies an area
        :return: a list of urls in the target area
        """
        if SYS_INFO.system.startswith('Windows'):
            chrome_options = webdriver.ChromeOptions()
            chrome_options.add_argument("--headless")
            chrome_options.add_argument("--user-agent=Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36")
            chrome_options.add_argument("--disable-gpu")
            chrome_options.add_argument("--disable-infobars")
            chromedriver_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__))) + os.sep + 'drivers' + os.sep + 'nt' + os.sep + "chromedriver"
            chrome_browser = webdriver.Chrome(chrome_options=chrome_options, executable_path=chromedriver_dir)
            all_links = list()
            try:
                chrome_browser.get(url)
                locator = (By.XPATH, target_xpath)
                wait = WebDriverWait(chrome_browser, 20, 0.5).until(ec.presence_of_element_located(locator))
                if wait:
                    try:
                        if stroll:
                            for i in range(15):
                                chrome_browser.execute_script('window.scrollTo(0,document.body.scrollHeight)')
                                time.sleep(0.5)
                            target_element_text = chrome_browser.find_element_by_xpath(target_xpath).get_attribute("outerHTML")
                            target_urls_tags = chrome_browser.find_element_by_xpath(target_xpath).find_elements_by_xpath(".//*[@href]")
                            titles = [i.get_attribute("href") for i in target_urls_tags]
                            all_links = list()
                            for tag in target_urls_tags:
                                link = tag.get_attribute("href")
                                if not link.startswith("javascript"):
                                    if not link.startswith("http"):
                                        link = urljoin(url, link)
                                    all_links.append(link)
                            all_links = remove_list_dup(all_links)
                            all_links = remove_non_article_link(all_links)
                            if len(all_links) > 100:
                                return all_links[:100]
                            else:
                                return all_links
                        else:
                            for i in range(4):
                                chrome_browser.execute_script('window.scrollTo(0,document.body.scrollHeight)')
                                time.sleep(0.5)
                            # get html text of the target area
                            target_element_text = chrome_browser.find_element_by_xpath(target_xpath).get_attribute("outerHTML")
                            # try to find tags contains links
                            target_urls_tags = chrome_browser.find_element_by_xpath(
                                target_xpath).find_elements_by_xpath(".//*[@href]")
                            titles = [i.get_attribute("href") for i in target_urls_tags]
                            all_links = list()
                            # collect all links into a list
                            for tag in target_urls_tags:
                                link = tag.get_attribute("href")
                                if not link.startswith("javascript"):
                                    if not link.startswith("http"):
                                        link = urljoin(url, link)
                                    all_links.append(link)
                            all_links = remove_list_dup(all_links)
                            all_links = remove_non_article_link(all_links)
                    except Exception as err:
                        logger.error(str(err) + url)
            except Exception as err:
                logger.error(str(err) + url)
            finally:
                chrome_browser.quit()
                logger.info("get {0} links from the target area: {1}".format(len(all_links), "  ".join(all_links)))
            return all_links
        else:
            # linux
            display = Display(visible=0, size=(800, 800))
            display.start()
            chrome_driver_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__))) + os.sep + 'drivers' + os.sep + 'posix' + os.sep + "chromedriver"
            options = webdriver.ChromeOptions()
            options.add_argument('--no-sandbox')
            options.add_argument('--disable-gpu')
            options.add_argument('--headless')
            options.add_argument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36")
            chrome_browser = webdriver.Chrome(chrome_options=options, executable_path=chrome_driver_dir)
            chrome_browser.set_page_load_timeout(300)
            all_links = list()
            try:
                chrome_browser.get(url)
                locator = (By.XPATH, target_xpath)
                wait = WebDriverWait(chrome_browser, 20, 0.5).until(ec.presence_of_element_located(locator))
                if wait:
                    try:
                        if stroll:
                            for i in range(15):
                                chrome_browser.execute_script('window.scrollTo(0,document.body.scrollHeight)')
                                time.sleep(0.5)
                            target_element_text = chrome_browser.find_element_by_xpath(target_xpath).get_attribute("outerHTML")
                            target_urls_tags = chrome_browser.find_element_by_xpath(target_xpath).find_elements_by_xpath(".//*[@href]")
                            titles = [i.get_attribute("href") for i in target_urls_tags]
                            for tag in target_urls_tags:
                                link = tag.get_attribute("href")
                                if not link.startswith("javascript"):
                                    if not link.startswith("http"):
                                        link = urljoin(url, link)
                                    all_links.append(link)
                            all_links = remove_list_dup(all_links)
                            all_links = remove_non_article_link(all_links)
                            if len(all_links) > 100:
                                return all_links[:100]
                            else:
                                return all_links
                        else:
                            for i in range(4):
                                chrome_browser.execute_script('window.scrollTo(0,document.body.scrollHeight)')
                                time.sleep(0.5)
                            # get html text of the target area
                            target_element_text = chrome_browser.find_element_by_xpath(target_xpath).get_attribute(
                                "outerHTML")
                            # try to find tags contains links
                            target_urls_tags = chrome_browser.find_element_by_xpath(
                                target_xpath).find_elements_by_xpath(".//*[@href]")
                            titles = [i.get_attribute("href") for i in target_urls_tags]
                            # collect all links into a list
                            for tag in target_urls_tags:
                                link = tag.get_attribute("href")
                                if not link.startswith("javascript"):
                                    if not link.startswith("http"):
                                        link = urljoin(url, link)
                                    all_links.append(link)
                            all_links = remove_list_dup(all_links)
                            all_links = remove_non_article_link(all_links)
                        logger.info("got {0} links from the target area are as follows: {1}".format(len(all_links), "  ".join(all_links)))
                    except Exception as err:
                        logger.error(str(err) + url)
            except Exception as err:
                logger.error(str(err) + url)
            finally:
                chrome_browser.quit()
            return all_links

    def get_articles(self):
        all_urls = self.target_urls
        articles = list()
        if all_urls:
            for url in all_urls:
                article = dict()
                article["url"] = url
                if SYS_INFO.system.startswith('Windows'):
                    chrome_options = webdriver.ChromeOptions()
                    chrome_options.add_argument("--headless")
                    chrome_options.add_argument("--user-agent=Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36")
                    chrome_options.add_argument("--disable-gpu")
                    chrome_options.add_argument("--disable-infobars")
                    chromedriver_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__))) + os.sep + 'drivers' + os.sep + 'nt' + os.sep + "chromedriver"
                    chrome_browser = webdriver.Chrome(chrome_options=chrome_options, executable_path=chromedriver_dir)
                    try:
                        chrome_browser.get(url)
                        # chrome_browser.execute_script('window.scrollTo(0,document.body.scrollHeight)')
                        locator = (By.XPATH, self.article_title_xpath)
                        wait = WebDriverWait(chrome_browser, 20, 0.5).until(ec.presence_of_element_located(locator))
                    except Exception as err:
                        logger.error(str(err) + url)
                    try:
                        title_element_text = chrome_browser.find_element_by_xpath(self.article_title_xpath).get_attribute("outerHTML")
                        content_element_text = chrome_browser.find_element_by_xpath(self.article_content_xpath).get_attribute("outerHTML")
                        # enerally real content text is in the p tag
                        if chrome_browser.find_element_by_xpath(self.article_content_xpath).find_elements_by_xpath(".//p"):
                            article["article_title"] = chrome_browser.find_element_by_xpath(self.article_title_xpath).text
                            article["article_content"] = [p.text for p in chrome_browser.find_element_by_xpath(self.article_content_xpath).find_elements_by_xpath(".//p")]
                            article["crawled_time"] = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
                            article["sched_id"] = self.sched_id
                        else:
                            article["article_title"] = chrome_browser.find_element_by_xpath(self.article_title_xpath).text
                            article["article_content"] = [chrome_browser.find_element_by_xpath(self.article_content_xpath).text]
                            article["crawled_time"] = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
                            article["sched_id"] = self.sched_id
                        articles.append(article)
                    except Exception as err:
                        logger.error(str(err) + url)
                    finally:
                        chrome_browser.quit()
                else:
                    # linux
                    display = Display(visible=0, size=(800, 800))
                    display.start()
                    chrome_driver_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__))) + os.sep + 'drivers' + os.sep + 'posix' + os.sep + "chromedriver"
                    options = webdriver.ChromeOptions()
                    options.add_argument('--no-sandbox')
                    options.add_argument('--disable-gpu')
                    options.add_argument('--headless')
                    options.add_argument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36")
                    chrome_browser = webdriver.Chrome(chrome_options=options, executable_path=chrome_driver_dir)
                    try:
                        chrome_browser.get(url)
                        # chrome_browser.execute_script('window.scrollTo(0,document.body.scrollHeight)')
                        locator = (By.XPATH, self.article_title_xpath)
                        wait = WebDriverWait(chrome_browser, 20, 0.5).until(ec.presence_of_element_located(locator))
                    except Exception as err:
                        logger.error(str(err) + url)
                    response_text = None
                    try:
                        title_element_text = chrome_browser.find_element_by_xpath(self.article_title_xpath).get_attribute("outerHTML")
                        content_element_text = chrome_browser.find_element_by_xpath(self.article_content_xpath).get_attribute("outerHTML")
                        # enerally real content text is in the p tag
                        if chrome_browser.find_element_by_xpath(self.article_content_xpath).find_elements_by_xpath(".//p"):
                            article["article_title"] = chrome_browser.find_element_by_xpath(self.article_title_xpath).text
                            article["article_content"] = [p.text for p in chrome_browser.find_element_by_xpath(self.article_content_xpath).find_elements_by_xpath(".//p")]
                            article["crawled_time"] = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
                            article["sched_id"] = self.sched_id
                        else:
                            article["article_title"] = chrome_browser.find_element_by_xpath(self.article_title_xpath).text
                            article["article_content"] = [chrome_browser.find_element_by_xpath(self.article_content_xpath).text]
                            article["crawled_time"] = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
                            article["crawled_time"] = self.sched_id
                        articles.append(article)
                    except Exception as err:
                        logger.error(str(err) + url)
                    finally:
                        chrome_browser.quit()
            logger.info("get {0} articles".format(len(all_urls)))
        return articles

    def pipeline_articles(self):
        # articles crawled
        articles = self.get_articles()
        # articles inserted_urls
        inserted_articles = list()
        # im_res = self.mo.bulkinsert(collectionname=self.info_col, data=articles)
        # inserted_ids = [str(o_id) for o_id in im_res.inserted_ids]
        # return inserted_ids
        if articles:
            for article in articles:
                res = self.mo.upinsert_one(collectionname=self.info_col, data=article, field_name="url")
                if not res['existing']:
                    inserted_articles.append(res["data"])
                    # files dir
                    files_dir = os.path.dirname(os.path.dirname(os.path.dirname(os.path.realpath(__file__)))) + os.path.sep + 'articles'
                    if not os.path.exists(files_dir):
                        try:
                            os.mkdir(files_dir)
                        except Exception as err:
                            logger.error(err)
                    try:
                        title = res["data"]["_id"]
                        with open(files_dir + os.path.sep + r'{}.txt'.format(title), "w", encoding="utf-8") as f:
                            f.write(res["data"]["article_title"] + "\n")
                            for c in res["data"]["article_content"]:
                                if c:
                                    f.write(c + "\n")
                    except Exception as err:
                        logger.error(err)
        return articles, inserted_articles
