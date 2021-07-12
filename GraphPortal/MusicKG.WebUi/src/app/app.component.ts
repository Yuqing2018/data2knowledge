import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import * as Ajv from 'ajv';
import { TEXT_SIMILARITY_SECHEMA, ENTITY_SECHEMA, TOKENIZATION_SECHEMA, WOKSPACR_TYPE_LIST, TEXT_PARAPHRASE_SECHEMA } from './core/common';

export const ajv = new Ajv();

@Component({
  selector: 'app-root',
  template: `
    <router-outlet></router-outlet>
  `
})
export class AppComponent implements OnInit {

  ngOnInit() {
    ajv.addSchema(TEXT_SIMILARITY_SECHEMA, WOKSPACR_TYPE_LIST[0].annotation);
    ajv.addSchema(ENTITY_SECHEMA, WOKSPACR_TYPE_LIST[1].annotation);
    ajv.addSchema(TOKENIZATION_SECHEMA, WOKSPACR_TYPE_LIST[2].annotation);
    ajv.addSchema(TEXT_PARAPHRASE_SECHEMA, WOKSPACR_TYPE_LIST[3].annotation);
  }

}
