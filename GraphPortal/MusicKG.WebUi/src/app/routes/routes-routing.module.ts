import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from '../layout/main/main.component';
import { AdminMainComponent } from '../layout/admin-main/admin-main.component';
import { UserAuthGuard } from '../core/user-auth.guard';
import { AnnotatorMainComponent } from '../layout/annotator-main/annotator-main.component';
import { RouterRedirectGuard } from 'src/app/core/router-redirect.guard';

const routes: Routes = [
    {
        path: '',
        component: MainComponent,
        canActivate: [UserAuthGuard, RouterRedirectGuard],
        // redirectTo: 'workspace',
        // pathMatch: 'full',
    },
    {
        path: 'workspace/:workspace',
        component: MainComponent,
        canActivate: [UserAuthGuard],
        children: [
            { path: 'collection', loadChildren: './collection/collection.module#CollectionModule' },
            { path: 'ontology', loadChildren: './ontology/ontology.module#OntologyModule' },
            { path: 'dialogue', loadChildren: './ontology-dialogue/ontology-dialogue.module#OntologyDialogueModule' },
            { path: 'document', loadChildren: './document/document.module#DocumentModule' },
            { path: 'task', loadChildren: './task/task.module#TaskModule' },
            { path: 'rule', loadChildren: './rule/rule.module#RuleModule' },
            { path: 'knowledge', loadChildren: './knowledge/knowledge.module#KnowledgeModule' },
            { path: 'dictionary', loadChildren: './dictionary/dictionary.module#DictionaryModule' }
        ]
    },
    {
        path: 'workspace',
        component: AdminMainComponent,
        canActivate: [UserAuthGuard],
        children: [
            { path: '', loadChildren: './workspace/workspace.module#WorkspaceModule' }
        ]
    },
    {
        path: 'user',
        component: AdminMainComponent,
        canActivate: [UserAuthGuard],
        children: [
            { path: '', loadChildren: './user/user.module#UserModule' }
        ]
    },
    {
        path: 'faq',
        component: AdminMainComponent,
        canActivate: [UserAuthGuard],
        children: [
            { path: '', loadChildren: './faq/faq.module#FaqModule' }
        ]
    },
    {
        path: 'annotator',
        component: AnnotatorMainComponent,
        canActivate: [UserAuthGuard],
        children: [
            { path: 'circular', loadChildren: './circular/circular.module#CircularModule' },
            { path: 'analysis', loadChildren: './analysis/analysis.module#AnalysisModule' },
            { path: '', loadChildren: './annotator/annotator.module#AnnotatorModule' },
        ]
    },
    {
        path: 'exception',
        component: AdminMainComponent,
        children: [
            { path: '', loadChildren: './exception/exception.module#ExceptionModule' }
        ]
    },
    {
        path: 'login',
        // canActivate: [UserAuthGuard, RouterRedirectGuard],
        children: [
            { path: '', loadChildren: './auth/auth.module#AuthModule' }
        ]
    },
    {
        path: 'statistic',
        component: AdminMainComponent,
        canActivate: [UserAuthGuard],
        children: [
            { path: '', loadChildren: './statistic/statistic.module#StatisticModule' }
        ]
    },
    {
        path: '**',
        redirectTo: 'login'
    }

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RoutesRoutingModule { }