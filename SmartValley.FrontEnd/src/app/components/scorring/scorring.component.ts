import {Component, OnInit} from '@angular/core';
import {Scorring} from '../../services/scorring';


@Component({
  selector: 'app-scorring',
  templateUrl: './scorring.component.html',
  styleUrls: ['./scorring.component.css']
})
export class ScorringComponent implements OnInit {

  public scorrings: Array<Scorring>;

  constructor() {
    this.initTestData();
  }

  // тестовые данные
  initTestData() {
    this.scorrings = new Array<Scorring>();
    this.scorrings.push(<Scorring>{
      projectName: 'Rega Risk Sharing',
      projectArea: 'Crowdsurance',
      projectCountry: 'Russia',
      scoringRating: 99,
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorrings.push(<Scorring>{
      projectName: 'BitClave Active Search Ecosystem',
      projectArea: 'Rotetechnology',
      projectCountry: 'Russia',
      scoringRating: 89,
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorrings.push(<Scorring>{
      projectName: 'B2Broker',
      projectArea: 'Brokering',
      projectCountry: 'Russia',
      scoringRating: 65,
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });
  }

  ngOnInit() {
  }

}
