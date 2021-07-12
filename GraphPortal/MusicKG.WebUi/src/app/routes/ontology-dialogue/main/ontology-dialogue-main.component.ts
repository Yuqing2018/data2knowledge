import { Component, OnInit } from '@angular/core';
import { unflatten } from 'src/app/core/utils';
import { HttpClient, HttpRequest, HttpEvent, HttpEventType, HttpResponse } from '@angular/common/http';
import { UploadXHRArgs, UploadFile } from 'ng-zorro-antd/upload';
import { Router, ActivatedRoute } from '@angular/router';
import { OntologyDialogueService } from 'src/app/services/ontology-dialogue.service';
import { switchMap, tap, map } from 'rxjs/operators';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { NzMessageService } from 'ng-zorro-antd';


@Component({
    selector: 'km-ontology-dialogue-main',
    templateUrl: './ontology-dialogue-main.component.html',
    styleUrls: ['./ontology-dialogue-main.component.less']
})
export class OntologyDialogueMainComponent implements OnInit {
    ontology: any;
    intentList: any;
    tabIndex = 0;
    workspaceId: string;
    entityId: string;
    intentId: string;
    loading = false;

    constructor(
        private http: HttpClient,
        private route: ActivatedRoute,
        private dialogue: OntologyDialogueService,
        private premark: PreMarkService,
        private msg: NzMessageService
    ) { }

    tabSelect({ index }: any) {
        this.tabIndex = index;
    }

    customReq = (item: UploadXHRArgs) => {
        // Create a FormData here to store files and other parameters.
        const formData = new FormData();
        // tslint:disable-next-line:no-any
        if (this.tabIndex == 1) {
            formData.append('intentFile', item.file as any);
        } else {
            formData.append('entityFile', item.file as any);
        }
        const req = new HttpRequest('POST', `/Workspace/${this.workspaceId}/DialogOntology/Content`, formData, {
            reportProgress: true,
            withCredentials: true
        });
        // Always returns a `Subscription` object. nz-upload would automatically unsubscribe it at correct time.
        return this.http.request(req).subscribe(
            (event: HttpEvent<any>) => {
                if (event.type === HttpEventType.UploadProgress) {
                    if (event.total! > 0) {
                        // tslint:disable-next-line:no-any
                        (event as any).percent = (event.loaded / event.total!) * 100;
                    }
                    item.onProgress!(event, item.file!);
                } else if (event instanceof HttpResponse) {
                    item.onSuccess!(event.body, item.file!, event);
                }
            },
            err => {
                item.onError!(err, item.file!);
            }
        );
    };

    ngOnInit() {
        this.refreshData();
    }

    refreshData() {
        this.route.pathFromRoot[1].params.pipe(
            map(p => p.workspace),
            tap(id => this.workspaceId = id),
            switchMap(w => this.dialogue.getOntology(w))
        ).subscribe((res: any) => {
            this.intentId = res.intentDocumentId;
            this.entityId = res.entityDocumentId;
            this.getEntityData();
            this.getIntentData();
        });
    }

    getEntityData() {
        if (this.entityId) {
            this.premark.downloadDocument(this.workspaceId, this.entityId).subscribe(res => {
                this.ontology = unflatten(res);
            });
        }
    }

    getIntentData() {
        if (this.intentId) {
            this.premark.downloadDocument(this.workspaceId, this.intentId).subscribe(res => {
                this.intentList = res;
            });
        }
    }

    handleChange(info: { file: UploadFile }): void {
        switch (info.file.status) {
            case 'uploading':
                this.loading = true;
                break;
            case 'done':
                this.msg.success('文件上传成功');
                this.refreshData();
                this.loading = false;
                break;
            case 'error':
                this.msg.error(info.file.error.msg);
                this.loading = false;
                break;
        }
    }

}
