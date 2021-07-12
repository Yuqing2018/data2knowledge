import { TestBed } from '@angular/core/testing';

import { OptionService } from './option.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('OptionService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule]  
  }));

  it('should be created', () => {
    const service: OptionService = TestBed.get(OptionService);
    expect(service).toBeTruthy();
  });
});
