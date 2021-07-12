import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OptionService } from '../services/option.service';

@Injectable()
export class StartupService {

    constructor(
        private optionService: OptionService
    ) { }

    load(): Promise<any> {
        return new Promise(resolve => {
            this.optionService.getAllOptions().pipe(
                catchError(err => {
                    resolve(null);
                    return [];
                })
            ).subscribe(res => {
                resolve(null);
            })
        });
    }
}