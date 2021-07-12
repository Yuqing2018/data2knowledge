import { WorkspaceTypePipe } from './workspace-type.pipe';
import { of } from 'rxjs';

describe('WorkspaceTypePipe', () => {
  it('create an instance', () => {
    let optionService = jasmine.createSpyObj('OptionService',['getWorkspaceType']);
    optionService.getWorkspaceType.and.returnValue(of([]));
    const pipe = new WorkspaceTypePipe(optionService);
    expect(pipe).toBeTruthy();
  });
});
