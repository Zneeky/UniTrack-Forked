import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { MaterialModule } from './shared/material.module';
import { AuthModule } from './auth/auth.module';
import { ProfileModule } from './profile/profile.module';
import { StatisticsModule } from './statistics/statistics.module';
import { SharedModule } from './shared/shared.module';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { StudentsListModule } from './students-list/students-list.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AdminModule } from './admin/admin.module';
import { ErrorInterceptor } from './shared/services/error-handler.interceptor';
import { ChatComponent } from './chat/chat/chat.component';
import { FormsModule } from '@angular/forms';  // Import FormsModule


@NgModule({
  declarations: [AppComponent, ChatComponent],
  imports: [
    BrowserModule,
    MaterialModule,
    AuthModule,
    ProfileModule,
    StatisticsModule,
    SharedModule,
    AppRoutingModule,
    StudentsListModule,
    AdminModule,
    HttpClientModule,
    FormsModule, 
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
