import { Component, OnInit, OnDestroy } from '@angular/core';
import { PageHeaderComponent } from '@shared';
import { MyVeranstaltungDummyQueryGQL } from '../../../../graphql/generated';


@Component({
  selector: 'app-veranstalterveranstaltungen',
  templateUrl: './veranstaltung-anlegen.component.html',
  styleUrls: ['./veranstaltung-anlegen.component.css'],
  standalone: true,
  imports: [PageHeaderComponent]
})
export class VeranstaltungAnlegenComponent implements OnInit, OnDestroy {

  public name = '';
  constructor(private readonly queryService: MyVeranstaltungDummyQueryGQL) { }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  ngOnInit() {
    this.queryService.watch().valueChanges.subscribe(
      result => this.name = result.data.veranstaltungDummy.name);
  }
}
