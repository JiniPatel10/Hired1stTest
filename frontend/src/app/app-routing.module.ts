import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { AuthGuard } from 'src/shared/auth-guard';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { UsersListComponent } from './components/users-list/users-list.component';
import { ProductsListComponent } from './components/products-list/products-list.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';


const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  { path: 'login', component: LoginComponent},
  { path: 'registration', component: RegistrationComponent },
  { path: 'reset-password', component: ForgotPasswordComponent },
  { path: 'change-password/:id', component: ChangePasswordComponent },
  { path: 'users', component: UsersListComponent, canActivate: [AuthGuard] },
  { path: 'products', component: ProductsListComponent, canActivate: [AuthGuard] },
  { path: 'profile', component: EditProfileComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '/',pathMatch: 'full' },
   { path: '', redirectTo: '/login', pathMatch: 'full' }, // Default route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
