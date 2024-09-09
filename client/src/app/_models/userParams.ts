import { User } from './user';

export class UserParams {
  gender: string;
  minAge = 18;
  maxAge?: number;
  pageNumber = 1;
  pageSize = 5;
  orderBy: 'lastActive' | 'created' = 'lastActive';

  constructor(user: User | null) {
    this.gender = user?.gender === 'female' ? 'male' : 'female';
  }
}
