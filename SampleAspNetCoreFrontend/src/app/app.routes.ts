import { Routes,Route } from '@angular/router';
import { PageHome } from './page-home/page-home';
import { PageAdministrationHome } from './administration/page-administration-home/page-administration-home';
import { PageUser } from './administration/page-user/page-user';
import { PageGrid } from './page-grid/page-grid';
export type AppRoute = Route & { data: { title: string, ShowInNavigation: boolean } };
export type AppRoutes = AppRoute[]
export const routes: AppRoutes = [
    {
        path: '',
        component: PageHome,
        pathMatch: 'full',
        data: { title: 'Home', ShowInNavigation: true } 
    },
    {
        path: 'administration',
        component: PageAdministrationHome,
        data: { title: 'Administration', ShowInNavigation: true },
        /*
        children: [
            {
                path: 'user',
                component: PageUser,
                data: { title: 'User - Administration', ShowInNavigation: true }   
            },
            {
                path: 'user/:id',
                component: PageUser,
                data: { title: 'User - Administration', ShowInNavigation: false }   
            }
        ]   
        */
    },
    
    {
        path: 'administration/user',
        component: PageUser,
        data: { title: 'User - Administration', ShowInNavigation: true }   
    },
    {
        path: 'administration/user/:id',
        component: PageUser,
        data: { title: 'User - Administration', ShowInNavigation: false }   
    },    
    {
        path: 'grid',
        component: PageGrid,
        data: { title: 'Grid', ShowInNavigation: true }
    }
];
