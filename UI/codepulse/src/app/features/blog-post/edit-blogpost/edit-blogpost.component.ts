import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})

export class EditBlogpostComponent implements OnInit, OnDestroy {
  id: string | null = null;
  model?: BlogPost;
  routeSubscription?: Subscription;

  constructor(private route: ActivatedRoute,
    private blogPostService: BlogPostService
  ) {
    
  }
  


  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        // get blogpost from api
        if(this.id) {
        this.blogPostService.getBlogPostById(this.id)
          .subscribe({
            next: (response) => {
              this.model = response;
            }
          });
        }
      }
    })
  }

  onFormSubmit(): void {}

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
  }
}
