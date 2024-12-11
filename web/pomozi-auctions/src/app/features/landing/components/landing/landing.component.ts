import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { LandingState } from '../../store/landing.state';
import { Observable } from 'rxjs';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { selectLoading } from '../../store/landing.selectors';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent {
  loading$!: Observable<boolean>;

  constructor(
    private store: Store<LandingState>,
  ) {
    this.loading$ = this.store.select(selectLoading);
  }

  ngOnInit(): void {
  }
}
