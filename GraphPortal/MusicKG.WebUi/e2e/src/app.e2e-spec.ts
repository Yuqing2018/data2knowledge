import { AppPage } from './app.po';
import { by, element } from 'protractor';

describe('workspace-project App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('login page', () => {
    // page.navigateTo('login');
    // expect(element(by.css('.form-title')).getText()).toEqual('密码登录');
  });

});
