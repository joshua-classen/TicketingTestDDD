import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';



//todo: ich glaube ich muss hier den authGuard so umschreiben, sodass der immer eien request an meinen Server schickt und der server sagt dann
// ob das cookie noch gültig ist oder nicht.
export const authGuard = (route?: ActivatedRouteSnapshot, state?: RouterStateSnapshot) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.check() ? true : router.parseUrl('/auth/login');
  // return true; //todo: hier noch überarbeiten.
};
