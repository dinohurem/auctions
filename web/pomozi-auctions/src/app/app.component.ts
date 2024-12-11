import { Component, Inject, Renderer2 } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from './root/root.state';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, DOCUMENT } from '@angular/common';
import { PageLayoutComponent } from "./core/page-layout/page-layout.component";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, PageLayoutComponent]
})

export class AppComponent {
  title = 'pomozi-auctions';

  constructor(
    private readonly store: Store<State>,
    private renderer: Renderer2,
    @Inject(DOCUMENT) private document: Document
  ) {
  }

  ngOnInit(): void {
  }
}
