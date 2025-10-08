export interface Post {
    id: string;
    title: string;
    description: string;
    author: string;
    createdAt: Date;
    imageUrl?: string;
};