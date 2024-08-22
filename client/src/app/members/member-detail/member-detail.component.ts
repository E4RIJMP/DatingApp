import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
})
export class MemberDetailComponent implements OnInit {
  private membersService = inject(MembersService);
  private route = inject(ActivatedRoute);
  member?: Member;
  images: GalleryItem[] = [];

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;

    this.membersService.getMember(username).subscribe({
      next: (member) => {
        this.member = member;
        this.images = member.photos.map(
          (photo) => new ImageItem({ src: photo.url, thumb: photo.url })
        );
        // const photos = [
        //   { url: 'https://randomuser.me/api/portraits/women/1.jpg' },
        //   { url: 'https://randomuser.me/api/portraits/women/2.jpg' },
        //   { url: 'https://randomuser.me/api/portraits/women/3.jpg' },
        // ];

        // this.images = photos.map(
        //   (photo) => new ImageItem({ src: photo.url, thumb: photo.url })
        // );
      },
    });
  }
}
