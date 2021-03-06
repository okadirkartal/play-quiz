import { Component } from '@angular/core'; 
import { ApiService } from '../../services/api.service';   

@Component({
    selector:'quizzes',
    templateUrl: './quizzes.component.html'
})

export class QuizzesComponent{

    quiz = {};
    quizzes;
    
    constructor(public api : ApiService) {} 

    ngOnInit(){
        this.api.getQuizzes().subscribe(res => {
          this.quizzes = res; 
        });
    } 
}