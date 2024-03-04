import { Component, OnInit } from '@angular/core';
import { PageHeaderComponent } from '@shared';

@Component({
  selector: 'app-veranstalterveranstaltungen',
  templateUrl: './veranstalterveranstaltungen.component.html',
  styleUrls: ['./veranstalterveranstaltungen.component.css'],
  standalone: true,
  imports: [PageHeaderComponent]
})
export class VeranstalterveranstaltungenComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
