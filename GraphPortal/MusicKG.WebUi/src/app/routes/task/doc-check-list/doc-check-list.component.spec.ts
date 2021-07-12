import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocCheckListComponent } from './doc-check-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { of } from 'rxjs';

let comp: DocCheckListComponent;
let fixture: ComponentFixture<DocCheckListComponent>;
let page: Page;
let testDoc;

describe('DocCheckListComponent', () => {

  beforeEach(async(() => {
    testDoc = { "totalCount": 18, "from": 0, "count": 18, "items": [{ "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe6415cfcc0001faa513", "name": "新建文本文档 (2).txt", "status": "Removed", "tags": ["1"], "uploadedAt": "2019-01-16T09:50:28.144Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe6e15cfcc0001faa514", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["2"], "uploadedAt": "2019-01-16T09:50:38.15Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa515", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.698Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa516", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.709Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa517", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.714Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa518", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.72Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa519", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.726Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa51a", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.732Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa51b", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.738Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa51c", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.744Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa51d", "name": "新建文本文档 ", "status": "Uploaded", "tags": ["3", "22"], "uploadedAt": "2019-01-16T09:51:18.751Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efe9615cfcc0001faa51e", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["3"], "uploadedAt": "2019-01-16T09:51:18.76Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efeac15cfcc0001faa51f", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["122"], "uploadedAt": "2019-01-16T09:51:40.167Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3efeac15cfcc0001faa520", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": ["122"], "uploadedAt": "2019-01-16T09:51:40.174Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c3fdd8e4c7c5d000171141e", "name": "TextSimilarity_SampleInput.json", "status": "Uploaded", "tags": ["\"222\""], "uploadedAt": "2019-01-17T01:42:38.001Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c4007d1ee46ee00019f1ee7", "name": "Technical Requirement Specification of MKG v0.3.docx", "status": "Uploaded", "tags": ["标签一"], "uploadedAt": "2019-01-17T04:42:57.3Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c40095cee46ee00019f1ee8", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": [null], "uploadedAt": "2019-01-17T04:49:32.002Z", "uploadedBy": "manager" }, { "workspaceId": "5c3ee047530b6300019204a5", "id": "5c4037b6e547120001af0ab3", "name": "新建文本文档 (2).txt", "status": "Uploaded", "tags": [null], "uploadedAt": "2019-01-17T08:07:18.83Z", "uploadedBy": "manager" }] };

    const preMarkService = jasmine.createSpyObj('PreMarkService', ['getCreateTaskDocumentList']);
    preMarkService.getCreateTaskDocumentList.and.returnValue(of(testDoc));

    TestBed.configureTestingModule({
      imports: [SharedModule],
      declarations: [DocCheckListComponent],
      providers: [
        { provide: PreMarkService, useValue: preMarkService }
      ]
    })
    .compileComponents()
    .then(createComponent);
  }));

  it('should create', () => {
    expect(comp).toBeTruthy();
  });

  it('should display tag list', () => {
    comp.type = 'tag';
    fixture.detectChanges();
    expect(page.tagRows.length).toBeGreaterThan(0); 
  });

  it('should display doc list when click doc btn', () => {
    page.docBtn.click();
    fixture.detectChanges();  
    let docRowNodes = fixture.nativeElement.querySelectorAll('.doc-ul li');
    let docRows = Array.from(docRowNodes);
    expect(docRows.length).toBeGreaterThan(0);
  });

  it('1st tag should match 1st test tag', () => {
    const expectedTag = testDoc.items[0].tags[0];
    const actualTag = page.tagRows[0].textContent;
    expect(actualTag).toContain(expectedTag, 'tag.id');
  });

});

function createComponent() {
  fixture = TestBed.createComponent(DocCheckListComponent);
  comp = fixture.componentInstance;
  fixture.detectChanges();

  return fixture.whenStable().then(() => {
    fixture.detectChanges();
    page = new Page(fixture);
  });
}

class Page {
  tagRows: HTMLElement[];
  docBtn: HTMLElement;

  constructor(fixture: ComponentFixture<DocCheckListComponent>) {
    const tagRowNodes = fixture.nativeElement.querySelectorAll('.tag-ul li');
    this.tagRows = Array.from(tagRowNodes);
    this.docBtn = fixture.nativeElement.querySelector('label[nzValue="doc"]');
  }
}
