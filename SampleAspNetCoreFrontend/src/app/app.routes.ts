import { Routes } from '@angular/router';
import { PageHome } from './page-home/page-home';
import { PageAdministrationHome } from './administration/page-administration-home/page-administration-home';

export const routes: Routes = [
    {
        path: '',
        component: PageHome
    },
    {
        path: 'home',
        component: PageHome
    },
    {
        path: 'administration',
        component: PageAdministrationHome
    }

];
