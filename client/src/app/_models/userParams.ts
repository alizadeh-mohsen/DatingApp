import { User } from "./User";

export class userParams {
    pageNumber = 1;
    pageSize = 10;
    gender: string;
    minAge = 18;
    maxAge = 100;
    orderBy='lastActive';

    constructor(user: User) {

        this.gender = user.gender === 'male' ? 'female' : 'male';

    }

}