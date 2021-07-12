import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { WorkspaceService } from 'src/app/services/workspace.service';

const Menu = [
  {
    title: 'Ontology管理',
    icon: 'share-alt',
    link: 'ontology',
    type: ['5c414c0a5a01cf00010f9661', '5ceb51576dcd8ca0327808e8']
  },
  {
    title: 'Ontology管理',
    icon: 'share-alt',
    link: 'dialogue',
    type: ['5d63a6b099b830d028fd436d']
  },
  {
    title: '标签管理',
    icon: 'share-alt',
    link: 'ontology',
    type: ['5d831ababab4cf767dcaabbf']
  },
  {
    title: '采集任务管理',
    icon: 'scan',
    link: 'collection',
    type: ['5ceb51576dcd8ca0327808e8']
  },
  {
    title: '文档管理',
    icon: 'file-text',
    link: 'document',
    type: ['5c414bd75a01cf00010f9660', '5c414c0a5a01cf00010f9661', '5c74b10d148dce0714010ed6', '5c74b117148dce0714010ed7', '5ceb51576dcd8ca0327808e8', '5d831ababab4cf767dcaabbf', '5faba9e42f2fb1c4d65e3cc3']
  },
  {
    title: '规则管理',
    icon: 'cluster',
    link: 'rule',
    type: ['5ceb51576dcd8ca0327808e8']
  },
  {
    title: '标注任务',
    icon: 'highlight',
    link: 'task',
    type: ['5c414bd75a01cf00010f9660', '5c414c0a5a01cf00010f9661', '5c74b10d148dce0714010ed6', '5c74b117148dce0714010ed7', '5ceb51576dcd8ca0327808e8', '5d63a6b099b830d028fd436d', '5d831ababab4cf767dcaabbf', '5faba9e42f2fb1c4d65e3cc3']
  },
  {
    title: '知识图谱',
    icon: 'deployment-unit',
    link: 'knowledge',
    type: ['5ceb51576dcd8ca0327808e8']
  },
  {
    title: '辅助资源',
    icon: 'book',
    link: 'dictionary',
    type: ['5c414bd75a01cf00010f9660', '5ceb51576dcd8ca0327808e8', '5c74b10d148dce0714010ed6', '5faba9e42f2fb1c4d65e3cc3']
  },
];

@Component({
  selector: 'km-sidebar-nav',
  templateUrl: './sidebar-nav.component.html',
  styleUrls: ['./sidebar-nav.component.less']
})
export class SidebarNavComponent implements OnInit {
  menuData: any;
  workspaceType: string;

  @Input() isCollapsed: boolean;

  constructor(
    private route: ActivatedRoute,
    private workspaceService: WorkspaceService
  ) { }

  ngOnInit() {
    this.route.pathFromRoot[1].params.pipe(
      switchMap(w => this.workspaceService.info(w.workspace))
    ).subscribe(res => {
      this.workspaceType = res.type.id;
      this.menuData = Menu.filter(m => m.type.some(t => t == this.workspaceType));
    });
  }

}
