

type PostProps = {
    id: number;
    title: string;
    content: string;
    imageUrl?: string;
}
export const Post = (props: PostProps) => {
const {id, title, content} = props;

    return (

        <div  key={id} className="w-full border-[1px] border-gray-300 p-4">
            <h2 className="text-xl font-bold mb-2">{title}</h2>
            <p>{content}</p>
        </div>
    )
}