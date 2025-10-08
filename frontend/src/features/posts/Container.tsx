import { Post } from "./Post"


export const PostList = () => {
    const mock = [
        {
            id: 1,
            title: "Post 1",
            content: "This is the content of post 1"
        },
        {
            id: 2,
            title: "Post 2",
            content: "This is the content of post 2"
        }
    ]
    return (
        <div className="dark:bg-medium-dark bg-neutro-200 w-full flex flex-col gap-4 px-6 min-h-dvh overflow-y-auto">
            {mock.map(post => (
                <Post
                    id={post.id}
                    title={post.title}
                    content={post.content}
                />
            ))}
        </div>
    )
}