// noinspection LanguageDetectionInspection

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { admin, LoginService, Menu } from '@core';
import { map } from 'rxjs/operators';
import { LoginVeranstalterGQL } from '../../graphql/generated';

@Injectable()
export class TicketVeranstalterLoginService extends LoginService {


  private token = { access_token: 'MW56YjMyOUAxNjMuY29tWm9uZ2Jpbg==', token_type: 'bearer' };


  constructor(
    protected http: HttpClient,
    private readonly mutationService: LoginVeranstalterGQL
  ) {
    super(http);
  }


  // todo: klären ob ich vll nicht diese Vererbung verwerfen sollte und stattdessen ein interface definieren sollte

  // Wieso wird diese Methode ausgeführt? Das liegt wahrscheinlich an dem
  // { provide: LoginService, useClass: FakeLoginService }, aus der app.config.ts
  login(username: string, password: string, rememberMe = false): Observable<string> {
    // führe die mutation loginVeranstalter ausführen
    // todo: ersmal grahql dateien erstellen und ablegen//todo: herausfinden wie das MD macht
    // todo: dann .ts code generieren
    // todo: dann mustation hier nutzen.

    return this.mutationService.mutate({ input: { email: username, password: password } })
      .pipe(
        map(result => result.data?.loginVeranstalter.email || '') // Extrahieren der E-Mail aus dem Ergebnis
      );
  }

  refresh() {
    return of(this.token);
  }

  logout() {
    return of({});
  }

  me() {
    return of(admin);
  }

  menu() {
    return this.http
      .get<{ menu: Menu[] }>('assets/data/menu.json?_t=' + Date.now())
      .pipe(map(res => res.menu));
  }
}
