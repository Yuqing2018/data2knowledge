import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { unflatten } from 'src/app/core/utils';
import { DropService } from 'src/app/services/drop.service';

@Component({
    selector: 'km-ontology-tree',
    templateUrl: './ontology-tree.component.html',
    styleUrls: ['./ontology-tree.component.less']
})
export class OntologyTreeComponent implements OnInit {
    ontologyTree: any;

    @Input() set ontology(ontology: any) {
        if (ontology) {
            this.ontologyTree = unflatten(ontology);
        }
    }

    @Output() selectEntity = new EventEmitter();

    constructor(
        private dropService: DropService
    ) { }

    ngOnInit() {
    }

    onDragstart(event: any, node: any) {
        this.dropService.dropEntity = node;
    }

    onClickEntity(event: MouseEvent, node: any) {
        this.selectEntity.emit(node);
        event.stopPropagation();
    }

}
