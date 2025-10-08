import { useState } from "react"
import ReactModal from "react-modal"

ReactModal.setAppElement('#root');
export const CreatePost = () => {
    const  [open, setOpen] = useState<boolean>(false);

    return (
        <>
            <div className="flex items-center p-4 gap-4 rounded-md border-[1px] border-gray-300 w-[80%]">
                <div className="rounded-full bg-minimal-dark w-full max-w-[4rem] aspect-square overflow-hidden flex items-center justify-center">
                    photo
                </div>
                <button className="dark:bg-minimal-dark bg-gray-300 p-3 rounded-4xl w-full text-start"
                 onClick={() => setOpen(true)}
                >
                    Nova publicação
                </button>
            </div>
            <ReactModal isOpen={open}
            onRequestClose={() => setOpen(false)}>
                <h2>teste</h2>
                <button onClick={() => setOpen(false)}>fechar</button>
            </ReactModal>
        </>
    )
}