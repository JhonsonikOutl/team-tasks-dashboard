import { APP_INITIALIZER, ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app.routes';
import { CatalogService } from './core/services/catalog.service';

function initCatalog(catalogService: CatalogService) {
  return () => catalogService.preload().toPromise();
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    {
      provide: APP_INITIALIZER,
      useFactory: initCatalog,
      deps: [CatalogService],
      multi: true
    }
  ]
};