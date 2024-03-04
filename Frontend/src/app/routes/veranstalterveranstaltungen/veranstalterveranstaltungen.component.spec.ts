import { waitForAsync, ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { PageHeaderComponent } from '@shared';

import { VeranstalterveranstaltungenComponent } from './veranstalterveranstaltungen.component';

describe('VeranstalterveranstaltungenComponent', () => {
  let component: VeranstalterveranstaltungenComponent;
  let fixture: ComponentFixture<VeranstalterveranstaltungenComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [NoopAnimationsModule, PageHeaderComponent]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VeranstalterveranstaltungenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
