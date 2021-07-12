import { WorkspaceLanguagePipe } from './workspace-language.pipe';
import { of } from 'rxjs';

describe('WorkspaceLanguagePipe', () => {
  it('create an instance', () => {
    let optionService = jasmine.createSpyObj('OptionService',['getLanguage']);
    optionService.getLanguage.and.returnValue(of([]));
    const pipe = new WorkspaceLanguagePipe(optionService);
    expect(pipe).toBeTruthy();
  });
});
