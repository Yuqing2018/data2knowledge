import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskUpdateFormComponent } from './task-update-form.component';
import { SharedModule } from 'src/app/shared/shared.module';

describe('TaskUpdateFormComponent', () => {
  let component: TaskUpdateFormComponent;
  let fixture: ComponentFixture<TaskUpdateFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ SharedModule ],
      declarations: [ TaskUpdateFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskUpdateFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
