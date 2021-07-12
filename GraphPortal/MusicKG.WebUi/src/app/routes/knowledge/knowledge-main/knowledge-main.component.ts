import { Component, OnInit,ElementRef } from '@angular/core';
import vis from 'vis';

@Component({
  selector: 'km-knowledge-main',
  templateUrl: './knowledge-main.component.html',
  styleUrls: ['./knowledge-main.component.less']
})
export class KnowledgeMainComponent implements OnInit {
  vis: vis;
  nodes:any[]=[];
  edges:any[]=[];
  attrArray:any[]=[{name:"属性1",value:"value"},{name:"属性2",value:"value2"}]

  constructor(
    private elementRef: ElementRef
  ) { }

  ngOnInit() {
    this.initVis();
  }

  initHtml (title:string,attrArray:any[]){
    var trStr=''
    attrArray.forEach(item=>{
      trStr+= `
        <tr style="border:1px solid #ddd;">
          <td style="border:1px solid #ddd;">${item.name}</td>
          <td style="border:1px solid #ddd;">${item.value}</td>
        </tr>
      `
    })
    let str=`
      <h3 style="text-align: center;">${title}</h3>
      <table style="border:1px solid #ddd;width:200px;text-align: center; border-collapse: collapse;">
        <tr style="border:1px solid #ddd;">
          <th style="border:1px solid #ddd;">属性</th>
          <th style="border:1px solid #ddd;">值</th>
        </tr>
       ${trStr}
      </table>
      `
    return str;

  }
  initVis () {
    let nodes = [{id: "哺乳动物", label: "哺乳动物", group: 0,title:this.initHtml('哺乳动物',this.attrArray)},
        {id: "脊椎", label: "脊椎", group: 0},
        {id: "老虎", label: "老虎", group: 0},
        {id: "毛", label: "毛", group: 1},
        {id: "猩猩", label: "猩猩", group: 1},
        {id: "鲸鱼", label: "鲸鱼", group: 1},
        {id: "海水", label: "海水", group: 0},
        {id: "鲨鱼", label: "鲨鱼", group: 2},
        {id: "动物", label: "动物", group: 2}
    ];
    let edges =[{id:1,from: "哺乳动物", to: "脊椎",title: '有',font: {align: 'top'},arrows:'to'},
        {from: "老虎", to: "哺乳动物",title: '是',font: {align: 'middle'},arrows:'to'},
        {from: "老虎", to: "毛",title: '有',font: {align: 'middle'},arrows:'to'},
        {from: "猩猩", to: "哺乳动物",title: '是',font: {align: 'middle'},arrows:'to'},
        {from: "猩猩", to: "毛",title: '有',font: {align: 'middle'},arrows:'to'},
        {from: "鲸鱼", to: "哺乳动物",title: '是',font: {align: 'middle'},arrows:'to'},
        {from: "鲸鱼", to: "海水",title: '生活在',font: {align: 'middle'},arrows:'to'},
        {from: "鲨鱼", to: "动物",title: '是',font: {align: 'middle'},arrows:'to'},
        {from: "鲨鱼", to: "海水",title: '生活在',font: {align: 'middle'},arrows:'to'},
        {from: "哺乳动物", to: "动物",title: '是',font: {align: 'middle'},arrows:'to'}
    ]
    // create a network
    let container = this.elementRef.nativeElement.childNodes[2].childNodes[2];
    // let container = this.elementRef.nativeElement.childNodes[1].childNodes[0];
    console.log(this.elementRef.nativeElement.childNodes[2].childNodes[2])
    let data = {
        nodes: nodes,
        edges: edges
    };
    let options = {
        autoResize: true,
        width:'',
        height:'600',
        nodes: {
            // shape: 'dot',
            size: 20,
            font: {
                size: 10
            },
            borderWidth: 1,
            shadow:true
        },
        edges: {
            width: 1,
            font: {
              size: 10,
          },
          shadow:true
        },
        interaction: {
          hover: true,
          dragNodes:false,//节点是否可拖拽
        }
    };
     new vis.Network(container, data, options);
  }

}
