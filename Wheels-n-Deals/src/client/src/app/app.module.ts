import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { AppComponent } from './app.component';
import { AnnouncementComponent } from './components/announcement/announcement.component';
import { HomeComponent } from './components/home/home.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthorPipe } from './author.pipe';
import { NavigationComponent } from './components/navigation/navigation.component';
import { RegisterComponent } from './components/register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module'; // Import ReactiveFormsModule
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { DatepipePipe } from './datepipe.pipe';
import { ProfileComponent } from './components/profile/profile.component';
import { AnnouncementDetailComponent } from './components/announcement-detail/announcement-detail.component';
@NgModule({
  declarations: [
    AppComponent,
    AnnouncementComponent,
    HomeComponent,
    AuthorPipe,
    NavigationComponent,
    RegisterComponent,
    LoginComponent,
    DatepipePipe,
    ProfileComponent,
    AnnouncementDetailComponent
  ],
  imports: [
    BrowserModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatCardModule,
    RouterModule,
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
