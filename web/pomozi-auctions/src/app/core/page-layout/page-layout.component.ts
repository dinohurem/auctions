import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LandingComponent } from "../../features/landing/components/landing/landing.component";

@Component({
  selector: 'page-layout',
  templateUrl: './page-layout.component.html',
  styleUrls: ['./page-layout.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule, LandingComponent],
})
export class PageLayoutComponent {

  constructor() { }

  ngOnInit(): void {
  }

}
