import { Component, OnInit } from '@angular/core';
import { PageHeaderComponent } from '@shared';

@Component({
  selector: 'app-veranstalterveranstaltungen',
  templateUrl: './veranstaltung-anlegen.component.html',
  styleUrls: ['./veranstaltung-anlegen.component.css'],
  standalone: true,
  imports: [PageHeaderComponent]
})
export class VeranstaltungAnlegenComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
