import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: ['',[Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      address: ['']
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerData = this.registerForm.value;
      this.authService.register(registerData);
      this.router.navigate(['/']);
    }
  }
}
