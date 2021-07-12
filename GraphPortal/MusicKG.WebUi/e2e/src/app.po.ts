import { browser } from 'protractor';

export class AppPage {

  navigateTo(pageUrl) {
    return browser.get('/' + pageUrl)
  }

}
