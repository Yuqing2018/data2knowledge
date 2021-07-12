import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'km-annotator-upload',
  templateUrl: './annotator-upload.component.html',
  styleUrls: ['./annotator-upload.component.less']
})
export class AnnotatorUploadComponent implements OnInit {
  type = 'GOV';
  fileName = '';
  uploadFile: any;

  @ViewChild('fileEl') fileEl: ElementRef;

  constructor() { }

  ngOnInit() {
  }

  selectedFile() {
      this.fileEl.nativeElement.click();
  }

  fileUpload() {
    if (this.fileEl.nativeElement.files && this.fileEl.nativeElement.files.length > 0) {
        this.fileName = this.fileEl.nativeElement.files[0].name;
        this.uploadFile = this.fileEl.nativeElement.files[0];
    }
  }

}
