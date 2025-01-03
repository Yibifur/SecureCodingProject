// import React from 'react'
// import { Table, Thead, Tbody, Tr, Th, Td } from "react-super-responsive-table"

// export default function List() {
//     return (
//         <React.Fragment>

//             <div className="table-rep-plugin">
//                 <div className="table-responsive mb-0" data-pattern="priority-columns">
//                     <AreYouSureModal
//                         dispatchAction={dispatchAction}
//                         isOpen={isAreYouSureModalOpen}
//                         handleOpen={handleSureModalOpen}
//                         handleClose={() => setIsAreYouSureModalOpen(false)}
//                         user={selectedUser}
//                         type={clickedButton}
//                         fetchAgain={fetchUsersAgain}
//                     />
//                     <Table className="table table-striped table-bordered">
//                         <Thead>
//                             <Tr>
//                                 {props.tableHeader.map((title, index) => {
//                                     return (
//                                         <Th>
//                                             <span key={index}>{title}</span>
//                                         </Th>
//                                     )
//                                 })}
//                             </Tr>
//                         </Thead>
//                         <Tbody>
//                             {props.items.map((item, index) => {

//                                 return (
//                                     <tr
//                                         className="user-register-table-row"
//                                         key={index}
//                                         onClick={e => handleUserClick(e, item)}
//                                     >
//                                         <td
//                                             className=""
//                                             style={{ height: "50px", width: "fit-content" }}
//                                         >
//                                             <Input style={{ marginTop: "12px" }} type="checkbox" onChange={(e) => handleCheckboxChange(e, item)} checked={isChecked(item.id)} />
//                                         </td>
//                                         <td
//                                             className=""
//                                             style={{ height: "50px", width: "fit-content" }}
//                                         >
//                                             <p className="pt-2 mb-0">{item.name}</p>
//                                         </td>
//                                         <td style={{ height: "50px" }}>
//                                             <p className="pt-2 mb-0">{item.surname}</p>
//                                         </td>
//                                         <td
//                                             style={{ height: "50px" }}
//                                             data-priority={index + 1}
//                                         >
//                                             <p className="pt-2 mb-0">{item.tcNumber}</p>
//                                         </td>
//                                         <td
//                                             style={{ height: "50px" }}
//                                             data-priority={index + 1}
//                                         >
//                                             <p className="pt-2 mb-0">{item.email}</p>
//                                         </td>




//                                         <td
//                                             className="d-flex"
//                                             style={{
//                                                 justifyContent: "flex-start",
//                                                 position: "relative",
//                                             }}
//                                         >
//                                             xxx
//                                         </td>
//                                     </tr>
//                                 )
//                             })}
//                         </Tbody>
//                     </Table>
//                 </div>
//             </div>
//         </React.Fragment>
//     )
// }
