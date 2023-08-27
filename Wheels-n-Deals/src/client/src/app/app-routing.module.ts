import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AnnouncementDetailComponent } from './components/announcement-detail/announcement-detail.component';
import { AddAnnouncementComponent } from './components/add-announcement/add-announcement.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'announcement/:id', component: AnnouncementDetailComponent },
  { path: 'add', component: AddAnnouncementComponent },
  { path: 'announcement/edit/:id', component: AddAnnouncementComponent },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(routes)],
})
export class AppRoutingModule {}
