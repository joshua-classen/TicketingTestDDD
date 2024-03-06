import { Component, OnInit, OnDestroy } from '@angular/core';
import { PageHeaderComponent } from '@shared';
import { gql } from '../../../__generated__/';

import { Subscription } from 'rxjs';
import { Apollo } from 'apollo-angular';



// https://the-guild.dev/graphql/apollo-angular/docs/data/queries
// https://www.apollographql.com/tutorials/lift-off-part1/09-defining-a-query



const VERANSTALTUNG = gql(`
  query {
    veranstaltungDummy{
      name
    }
  }
`);

@Component({
  selector: 'app-veranstalterveranstaltungen',
  templateUrl: './veranstaltung-anlegen.component.html',
  styleUrls: ['./veranstaltung-anlegen.component.css'],
  standalone: true,
  imports: [PageHeaderComponent]
})
export class VeranstaltungAnlegenComponent implements OnInit, OnDestroy {
  loading: boolean = false; //todo: hier muss ich das initialisieren. Wie kann ich das auch uninitialized lassen?
  posts: any;

  private querySubscription: Subscription;

  constructor(private apollo: Apollo) {}

  ngOnInit() {
    this.querySubscription = this.apollo
      .watchQuery<any>({
        query: VERANSTALTUNG,
      })
      .valueChanges.subscribe(({ data, loading }) => {
        this.loading = loading;
        this.posts = data.posts;
      });
  }

  ngOnDestroy() {
    this.querySubscription.unsubscribe();
  }
}
