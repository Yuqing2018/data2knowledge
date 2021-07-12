import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { switchMap } from 'rxjs/operators';
import { forkJoin } from 'rxjs';
import { PreMarkService } from './pre-mark.service';

@Injectable({
    providedIn: 'root'
})
export class OntologyDialogueService {

    constructor(
        private http: HttpClient
    ) { }

    getOntology(workspaceId: string) {
        return this.http.get(`/Workspace/${workspaceId}/DialogOntology`)
    }

}