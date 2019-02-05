import { TestBed } from '@angular/core/testing';

import { StaticdataService } from './staticdata.service';

describe('StaticdataService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StaticdataService = TestBed.get(StaticdataService);
    expect(service).toBeTruthy();
  });
});
