import { TestBed, inject } from '@angular/core/testing';

import { SnackBarsServiceService } from './snack-bars-service.service';

describe('SnackBarsServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SnackBarsServiceService]
    });
  });

  it('should be created', inject([SnackBarsServiceService], (service: SnackBarsServiceService) => {
    expect(service).toBeTruthy();
  }));
});
