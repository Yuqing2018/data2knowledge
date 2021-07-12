import { TestBed } from '@angular/core/testing';

import { WorkspaceService } from './workspace.service';
import { HttpClient } from '@angular/common/http';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { Workspace } from '../interfaces/workspace.interface';

describe('WorkspaceService', () => {
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
  let service: WorkspaceService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    })
    httpClient = TestBed.get(HttpClient);
    httpTestingController = TestBed.get(HttpTestingController);
    service = TestBed.get(WorkspaceService);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('get workspace list', () => {
    service.getList().subscribe(); 
    const req = httpTestingController.expectOne('/Workspace');
    expect(req.request.method).toEqual('GET');
    req.flush({}); 
  });

  it('add workspace', () => {
    service.add(<Workspace>{}).subscribe();
    const req = httpTestingController.expectOne('/Workspace');
    expect(req.request.method).toEqual('POST');
    req.flush({});
  })

  it('delete workspace', () => {
    service.delete('id').subscribe();
    const req = httpTestingController.expectOne('/Workspace/id');
    expect(req.request.method).toEqual('DELETE');
    req.flush('success');
  })

  it('update workspace', () => {
    service.update(<Workspace> { id: '2' }).subscribe();
    const req = httpTestingController.expectOne('/Workspace/2');
    expect(req.request.method).toEqual('PUT');
    req.flush('success');
  })

});
