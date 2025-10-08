import { Header } from "@components/partials/Header"
import { PostList } from "@features/posts/Container"
import { CreatePost } from "@features/posts/CreatePost"

export const Home = () => {
    

    return (
        <>
        <Header />
        <div className="flex">
            <div className="flex flex-col sticky top-[4rem] border-r border-gray-300 h-[calc(100vh-4rem)] px-6 py-4 w-1/4 ">
                <h2>Lista de amizade</h2>
            </div>

            <div className="flex flex-col items-center w-3/4 pt-12 gap-6 overflow-y-auto h-[calc(100vh-4rem)]">
                <CreatePost />
                <PostList />
            </div>
        </div>
        </>
    )
}