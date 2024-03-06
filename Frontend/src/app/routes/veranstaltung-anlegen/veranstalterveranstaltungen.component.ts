import { Component, OnInit, OnDestroy } from '@angular/core';
import { PageHeaderComponent } from '@shared';
import { gql } from '../../../__generated__/';

import { Subscription } from 'rxjs';



//todo: einfache query im backend erstellen.


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

  constructor() { }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  ngOnInit() {
  }
}

// import { Component, OnInit, OnDestroy } from '@angular/core';
// // import { MyFeedGQL, MyFeedQuery } from './graphql'; // https://the-guild.dev/graphql/codegen/plugins/typescript/typescript-apollo-angular ganz unten
// // funktioniert nicht.
//
// // BE SURE TO USE Observable from rxjs and not from @apollo/client/core when using map
// import { Observable } from 'rxjs'
// import { map } from 'rxjs/operators'
//
// @Component({
//   selector: 'feed',
//   template: `
//     <h1>Feed:</h1>
//     <ul>
//       <li *ngFor="let item of feed | async">{{ item.id }}</li>
//     </ul>
//   `
// })
// export class FeedComponent {
//   feed: Observable<MyFeedQuery['feed']>
//
//   constructor(feedGQL: MyFeedGQL) {
//     this.feed = feedGQL.watch().valueChanges.pipe(map(result => result.data.feed))
//   }
// }
