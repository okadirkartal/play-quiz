import { Component } from '@angular/core'; 
import { FormBuilder,Validators } from '@angular/forms'; 
import { AuthService } from 'src/app/services/auth.service';

@Component({
     templateUrl: './login.component.html'
})

export class LoginComponent{
 
    form;

     constructor(public auth : AuthService,private fb : FormBuilder) {
         this.form = fb.group({
             email :['',Validators.required],
             password : ['',Validators.required]
         })
     }  
}