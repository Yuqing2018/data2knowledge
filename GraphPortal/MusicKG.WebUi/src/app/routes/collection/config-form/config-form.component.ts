import { Component, OnInit,Input } from '@angular/core';
import { FormBuilder,  FormGroup,  Validators} from '@angular/forms';

@Component({
  selector: 'km-config-form',
  templateUrl: './config-form.component.html',
  styleUrls: ['./config-form.component.less']
})
export class ConfigFormComponent implements OnInit {
  validateForm: FormGroup;
  areaForm:Array<FormGroup>=[];//验证区域的数组
  areaArray:Array<{area_name:string, is_stroll:boolean,  page_url:string, target_xpath:string, article_title_xpath:string, article_content_xpath:string}>=[]
  @Input() data: any;//编辑时传来的数据
  id:string = '';
  
  constructor(
    private fb: FormBuilder
  ) {   }

  ngOnInit() {
    // console.log(this)
    this.addConfig()
    if(this.data !== undefined){
      this.id = this.data._id;
      this.editAddArea()
    }else{
      this.addArea()
    }
  }

  addConfig():void{
    this.validateForm = this.fb.group({
      configName: [ this.data !== undefined ? this.data.conf_name : '', [ Validators.required ] ],
      desc   : [ this.data !== undefined ? this.data.desc : '']
    });

  }
  // 增加采集区域
  addArea(): void{
    let obj={
      area_name:'',
      is_stroll:false,
      page_url:'',
      target_xpath:'',
      article_title_xpath:'',
      article_content_xpath:''
    }
    this.areaArray.push(obj);
    this.areaArray.forEach((item,i)=>{
      // console.log(this.areaForm[i])
      if(this.areaForm[i]!==undefined){
        this.areaForm[i] = this.fb.group({
          area_name:[ this.areaForm[i].value.area_name, [ Validators.required ] ],
          is_stroll:[this.areaForm[i].value.is_stroll],
          page_url: [ this.areaForm[i].value.page_url, [ Validators.required ] ],
          target_xpath:[this.areaForm[i].value.target_xpath, [ Validators.required ] ],
          article_title_xpath:[this.areaForm[i].value.article_title_xpath, [ Validators.required ] ],
          article_content_xpath:[this.areaForm[i].value.article_content_xpath, [ Validators.required ] ]
        })
      }else{
        this.areaForm[i] = this.fb.group({
          area_name:[ '', [ Validators.required ] ],
          is_stroll:[false],
          page_url: [ '', [ Validators.required ] ],
          target_xpath:['', [ Validators.required ] ],
          article_title_xpath:['', [ Validators.required ] ],
          article_content_xpath:['', [ Validators.required ] ]
        })
      }
    })
  }
  editAddArea () :void{
    this.areaArray = this.data.items;
    this.areaArray.forEach((item,i)=>{
      // console.log(this.areaForm[i])
        this.areaForm[i] = this.fb.group({
          area_name:[ this.data.items[i].area_name, [ Validators.required ] ],
          is_stroll:[this.data.items[i].is_stroll],
          page_url: [ this.data.items[i].page_url, [ Validators.required ] ],
          target_xpath:[this.data.items[i].target_xpath, [ Validators.required ] ],
          article_title_xpath:[this.data.items[i].article_title_xpath, [ Validators.required ] ],
          article_content_xpath:[this.data.items[i].article_content_xpath, [ Validators.required ] ]
        })
    })
  }
// 删除一个区域
  delArea(index:number):void{
    if(this.areaArray.length>1){
      this.areaArray.splice(index,1);
      this.areaForm.splice(index,1);
    }
  }

  hasError(control: string , i:number) {
    return this.areaForm[i].get(control).dirty && this.areaForm[i].get(control).errors;
  }
}
