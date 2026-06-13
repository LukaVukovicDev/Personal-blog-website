import {
  afterNextRender,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  ElementRef,
  HostListener,
  inject,
  signal,
  viewChild,
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PostService } from '../../../core/services/post.service';
import { PostListItem } from '../../../core/models/post.model';

interface Topic {
  img: string;
  alt: string;
  title: string;
}

interface TagChip {
  img: string;
  label: string;
}

interface Popular {
  img: string;
  alt: string;
  title: string;
  read: string;
  date: string;
  datetime: string;
}

@Component({
  selector: 'app-home',
  imports: [RouterLink, DatePipe],
  templateUrl: './home.html',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class Home {
  private readonly postService = inject(PostService);

  private readonly slider = viewChild<ElementRef<HTMLElement>>('slider');
  private readonly sliderContainer = viewChild<ElementRef<HTMLElement>>('sliderContainer');

  private slidableItems = 0;
  private currentPos = 0;

  // ── Dinamički sadržaj iz API-ja ──────────────────────────────
  readonly featured = signal<PostListItem[]>([]);
  readonly recent = signal<PostListItem[]>([]);
  readonly loading = signal(true);

  // ── Dekorativni (statički) elementi dizajna ──────────────────
  readonly topics: Topic[] = [
    { img: 'assets/images/cy.png', alt: 'Cybersec', title: 'Cybersec' },
    { img: 'assets/images/machine_learning.jpg', alt: 'AI', title: 'AI' },
    { img: 'assets/images/linux.avif', alt: 'Linux', title: 'Linux' },
    { img: 'assets/images/wrt_htb.png', alt: 'Writeups', title: 'Writeups' },
    { img: 'assets/images/ctf.png', alt: 'CTF', title: 'CTF' },
  ];

  readonly tags: TagChip[] = [
    { img: 'assets/images/java-removebg-preview.png', label: 'Java' },
    { img: 'assets/images/chrome.png', label: 'Web dev' },
    { img: 'assets/images/ctf.png', label: 'CTF' },
    { img: 'assets/images/writeups.png', label: 'Writeups' },
    { img: 'assets/images/linux.png', label: 'Linux' },
    { img: 'assets/images/lock.png', label: 'AI' },
    { img: 'assets/images/code.jpg', label: 'Cybersec' },
    { img: 'assets/images/windows.png', label: 'Windows' },
    { img: 'assets/images/htb.png', label: 'HTB' },
    { img: 'assets/images/pentest.png', label: 'Pentest+' },
    { img: 'assets/images/oscp.png', label: 'OSCP' },
    { img: 'assets/images/wifi.png', label: 'Wifi' },
  ];

  readonly popular: Popular[] = [
    {
      img: 'assets/images/linux.png',
      alt: 'Posts about linux',
      title: 'Posts about linux',
      read: '15 mins read',
      date: '15 April 2024',
      datetime: '2024-04-15',
    },
    {
      img: 'assets/images/ctf.png',
      alt: 'CTF stories',
      title: 'CTF stories',
      read: '15 mins read',
      date: '15 May 2024',
      datetime: '2024-05-15',
    },
    {
      img: 'assets/images/windows.png',
      alt: 'Posts about windows',
      title: 'Posts about windows',
      read: '10 mins read',
      date: '15 May 2024',
      datetime: '2024-05-15',
    },
    {
      img: 'assets/images/wifi.png',
      alt: 'Wifi tricks and tips',
      title: 'Wifi tricks and tips',
      read: '15 mins read',
      date: '15 April 2024',
      datetime: '2024-04-15',
    },
    {
      img: 'assets/images/writeups.png',
      alt: 'Writeups for thm rooms and ctfs',
      title: 'Writeups for thm rooms and ctfs',
      read: '15 mins read',
      date: '15 April 2024',
      datetime: '2024-04-15',
    },
  ];

  constructor() {
    this.loadPosts();

    // DOM merenja slidera idu samo u browseru, nakon prvog renderovanja.
    afterNextRender(() => this.recalculate());
  }

  next(): void {
    this.currentPos = this.currentPos >= this.slidableItems ? 0 : this.currentPos + 1;
    this.moveSlider();
  }

  prev(): void {
    this.currentPos = this.currentPos <= 0 ? this.slidableItems : this.currentPos - 1;
    this.moveSlider();
  }

  @HostListener('window:resize')
  onResize(): void {
    this.recalculate();
  }

  private loadPosts(): void {
    // Prvih 5 objavljenih → "Posts", narednih 5 → "Recent posts".
    this.postService.getPosts({ page: 1, pageSize: 10 }).subscribe({
      next: (result) => {
        this.featured.set(result.items.slice(0, 5));
        this.recent.set(result.items.slice(5, 10));
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  private recalculate(): void {
    const slider = this.slider()?.nativeElement;
    const container = this.sliderContainer()?.nativeElement;
    if (!slider || !container) {
      return;
    }
    const visible = Number(getComputedStyle(slider).getPropertyValue('--slider-items')) || 1;
    this.slidableItems = Math.max(container.childElementCount - visible, 0);
    this.currentPos = Math.min(this.currentPos, this.slidableItems);
    this.moveSlider();
  }

  private moveSlider(): void {
    const container = this.sliderContainer()?.nativeElement;
    const child = container?.children[this.currentPos] as HTMLElement | undefined;
    if (container && child) {
      container.style.transform = `translateX(-${child.offsetLeft}px)`;
    }
  }
}
