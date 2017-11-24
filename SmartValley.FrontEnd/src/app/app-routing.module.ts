import {Paths} from './paths';
import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {MetamaskHowtoComponent} from './components/metamask-howto/metamask-howto.component';
import {RootComponent} from './components/root/root.component';
import {ApplicationComponent} from './components/application/application.component';
import {ScoringComponent} from './components/scoring/scoring.component';
import {EstimateComponent} from './components/estimate/estimate.component';
import {ReportComponent} from './components/report/report.component';


const appRoutes: Routes = [
  {path: Paths.Root, pathMatch: 'full', component: RootComponent},
  {path: Paths.MetaMaskHowTo, pathMatch: 'full', component: MetamaskHowtoComponent},
  {path: Paths.Scoring, pathMatch: 'full', component: ScoringComponent},
  {path: Paths.Application, pathMatch: 'full', component: ApplicationComponent},
  {path: Paths.Report + '/:id', pathMatch: 'full', component: ReportComponent},
  {path: Paths.Scoring + '/:id', pathMatch: 'full', component: EstimateComponent}
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {
}
